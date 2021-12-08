using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mediapipe.BlazePose;
using UnityEngine;
using UnityEngine.Events;

public class PoseExtractor : MonoBehaviour
{
  [StructLayout(LayoutKind.Sequential), Serializable]
  public struct PredictedPoint
  {
    public Vector3 position;
    public float confidence;
  }

  public enum PointType
  {
    LandmarkPoints,
    WorldPoints
  }

  private const byte NOSE_ID = 0;
  private const byte LEFT_WRIST_ID = 15;
  private const byte RIGHT_WRIST_ID = 16;
  private const byte LEFT_ANKLE_ID = 27;
  private const byte RIGHT_ANKLE_ID = 28;

  [Header("References")]
  public BlazePoseResource blazePoseResource;

  [Header("Parameters")]
  [Range(0, 1)]
  public float humanExistThreshold = 0.5f;

  public BlazePoseModel poseLandmarkModel;
  public PointType pointType;

  [Header("Outputs")]
  public PredictedPoint[] points;

  public float currentConfidence;
  public float confidenceStability;
  public bool found;

  public UnityEvent OnHumanProbablyFound;

  public UnityEvent OnHumanFound;
  public UnityEvent OnHumanLost;

  [Header("Events")]
  public UnityEvent<Vector3> OnHeadTracked;

  public UnityEvent<Vector3> OnLeftWristTracked;
  public UnityEvent<Vector3> OnRightWristTracked;
  public UnityEvent<Vector3> OnLeftAnkleTracked;
  public UnityEvent<Vector3> OnRightAnkleTracked;

  private BlazePoseDetecter detector;

  private bool maybeFound;
  private bool prevMaybeFound;
  private float counter;

  public PredictedPoint Head => points[NOSE_ID];
  public PredictedPoint LeftWrist => points[LEFT_WRIST_ID];
  public PredictedPoint RightWrist => points[RIGHT_WRIST_ID];
  public PredictedPoint LeftAnkle => points[LEFT_ANKLE_ID];
  public PredictedPoint RightAnkle => points[RIGHT_ANKLE_ID];

  private CircularBuffer<float> confidenceBuffer;

  private void Start()
  {
    detector = new BlazePoseDetecter(blazePoseResource, poseLandmarkModel);

    points = new PredictedPoint[34];

    Application.targetFrameRate = 60;
    confidenceBuffer = new CircularBuffer<float>(60);
  }

  public void OnImageReceived(Texture tex)
  {
    detector.ProcessImage(tex, poseLandmarkModel);

    if (pointType == PointType.WorldPoints)
    {
      detector.worldLandmarkBuffer.GetData(points);
    }
    else
    {
      detector.outputBuffer.GetData(points);
    }

    confidenceBuffer.Put(points[33].position.x);
    confidenceStability = ConfidenceStability();
    currentConfidence = confidenceBuffer.Average();


    if (Head.confidence >= 0.1f)
      OnHeadTracked.Invoke(Head.position);

    if (LeftWrist.confidence >= 0.1f)
      OnLeftWristTracked.Invoke(LeftWrist.position);

    if (RightWrist.confidence >= 0.1f)
      OnRightWristTracked.Invoke(RightWrist.position);

    if (LeftAnkle.confidence >= 0.1f)
      OnLeftAnkleTracked.Invoke(LeftAnkle.position);

    if (RightAnkle.confidence >= 0.1f)
      OnRightAnkleTracked.Invoke(RightAnkle.position);

    // We have maybe found someone if the current confidence momentarily exceeds the threshold
    // But we can't be sure its not noise
    // So we need to wait and observe
    maybeFound = currentConfidence >= humanExistThreshold;

    // Observation aligns with status
    // No more action needed
    if (maybeFound == found)
    {
      prevMaybeFound = maybeFound;
      return;
    }

    // Observation is different than status
    // If it's the first time our observation changed,
    // reset the timers and trigger the appropriate event
    if (maybeFound != prevMaybeFound)
    {
      counter = 0;

      if (maybeFound)
      {
        OnHumanProbablyFound.Invoke();
      }
    }
    // Keep observing, until the same observation is present for more than 2 seconds
    // Then trigger with confidence
    else
    {
      counter += Time.deltaTime;

      if (counter > 1)
      {
        if (maybeFound)
        {
          OnHumanFound.Invoke();
        }
        else if (maybeFound == false)
        {
          OnHumanLost.Invoke();
        }

        counter = 0;
        found = maybeFound;
      }
    }

    prevMaybeFound = maybeFound;
  }


  public float ConfidenceStability()
  {
    var confs = confidenceBuffer.ToArray();

    var sumDif = 0f;

    for (var i = 0; i < confs.Length - 1; i++)
    {
      sumDif += Mathf.Abs(confs[i] - confs[i + 1]);
    }

    return 1 - (sumDif / confs.Length);
  }
}
