using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildingPart
{
  public BuildingPart(float xValue = 0f, float yValue = 0f, float widthValue = 0f, float lengthValue = 0f)
  {
    x = xValue;
    y = yValue;
    width = widthValue;
    length = lengthValue;
  }
  public float x;
  public float y;
  public float width;
  public float length;
}



public class generate_building : MonoBehaviour
{
  private GameObject cube1, cube2;
  public Vector2 areaSize = new Vector2(100f, 100f);
  public float buidlingMinHeight = 30f;

  List<float> GenerateCellsPositions(float maxDistance = 0f)
  {
    List<float> arr = new List<float> { };
    float minCellSize = maxDistance / 10f;
    float cellSizeDiff = minCellSize / 1.2f;
    float currentCellSize = minCellSize + Random.value * cellSizeDiff;
    float currentPosition = 0f;
    float cellEnd = currentPosition + currentCellSize;
    while (cellEnd < maxDistance)
    {
      arr.Add(currentPosition);
      currentPosition = cellEnd;
      currentCellSize = minCellSize + Random.value * cellSizeDiff;
      cellEnd += currentCellSize;
    }
    return arr;
  }

  List<BuildingPart> GenerateBuildingParts(List<float> rows, List<float> columns)
  {
    List<BuildingPart> arr = new List<BuildingPart> { };
    for (int i = 1; i < columns.Count; i++)
    {
      for (int j = 1; j < rows.Count; j++)
      {
        if (Random.value > 0.3f)
        {

          float x = columns[i - 1];
          float y = rows[j - 1];

          float width = columns[i] - x;
          float length = rows[j] - y;

          BuildingPart b = new BuildingPart(x, y, width, length);

          cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
          cube1.transform.parent = gameObject.transform;
          
          cube1.transform.localScale = new Vector3(width, buidlingMinHeight, length);
          // cube1.transform.localScale = new Vector3(1f, buidlingMinHeight, 1f);
          cube1.transform.position = new Vector3(x + width / 2f, buidlingMinHeight / 2f, y + length / 2f);
          cube1.GetComponent<Renderer>().material.color = Color.white;

          arr.Add(b);
        }
      }
    }
    return arr;
  }

  void Start()
  {
    List<float> columnsPositions = GenerateCellsPositions(areaSize.x);
    List<float> rowsPositions = GenerateCellsPositions(areaSize.y);
    GenerateBuildingParts(rowsPositions, columnsPositions);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
