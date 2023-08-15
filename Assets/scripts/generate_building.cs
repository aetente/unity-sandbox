using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildingPart
{
  public BuildingPart(float xValue = 0f, float yValue = 0f, float widthValue = 0f, float lengthValue = 0f, bool isValidValue = true)
  {
    x = xValue;
    y = yValue;
    width = widthValue;
    length = lengthValue;
    isValid = isValidValue;
  }
  public float x;
  public float y;
  public float width;
  public float length;
  public bool isValid;
}



public class generate_building : MonoBehaviour
{
  private GameObject cube1, cube2;
  public Vector2 areaSize = new Vector2(50f, 50f);
  public float buidlingMinHeight = 5f;
  public int floors = 5;
  public float cellsPerRow = 10f;

  List<float> GenerateCellsPositions(float maxDistance = 0f)
  {
    List<float> arr = new List<float> { };
    float minCellSize = maxDistance / cellsPerRow;
    float cellSizeDiff = minCellSize / 1.2f;
    
    float currentCellSize = minCellSize + Random.Range(-1.0f, 1.0f) * cellSizeDiff;
    float currentPosition = 0f;
    float cellEnd = currentPosition + currentCellSize;
    while (cellEnd < maxDistance)
    {
      arr.Add(currentPosition);
      currentPosition = cellEnd;
      currentCellSize = minCellSize + Random.Range(-1.0f, 1.0f) * cellSizeDiff;
      cellEnd += currentCellSize;
    }
    return arr;
  }

  List<List<List<BuildingPart>>> GenerateBuildingParts(List<float> rows, List<float> columns)
  {
    List<List<List<BuildingPart>>> buildingFloors = new List<List<List<BuildingPart>>> { };
    List<List<BuildingPart>> arr = new List<List<BuildingPart>> { };
    List<BuildingPart> buildingPartsRow = new List<BuildingPart> { };
    for (int floor = 0; floor < floors; floor++)
    {
      for (int i = 1; i < columns.Count; i++)
      {
        for (int j = 1; j < rows.Count; j++)
        {
          BuildingPart b;
          if (Random.value > 0.3f)
          {

            float x = columns[i - 1];
            float y = rows[j - 1];

            float width = columns[i] - x;
            float length = rows[j] - y;

            b = new BuildingPart(x, y, width, length);

            cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.parent = gameObject.transform;

            cube1.transform.localScale = new Vector3(width, buidlingMinHeight, length);
            // cube1.transform.localScale = new Vector3(1f, buidlingMinHeight, 1f);
            cube1.transform.position = new Vector3(x + width / 2f, buidlingMinHeight / 2f * (2 * floor + 1), y + length / 2f);
            cube1.GetComponent<Renderer>().material.color = Color.white;

          }
          else
          {
            b = new BuildingPart(0f, 0f, 0f, 0f, false);
          }
          buildingPartsRow.Add(b);
        }
        arr.Add(buildingPartsRow);
        buildingPartsRow = new List<BuildingPart> { };
      }
      buildingFloors.Add(arr);
      arr = new List<List<BuildingPart>> { };
    }

    for (int floor = 0; floor < buildingFloors.Count; floor++)
    {
      for (int i = 0; i < buildingFloors[floor].Count; i++)
      {
        for (int j = 0; j < buildingFloors[floor][i].Count; j++)
        {

          if (buildingFloors[floor][i][j].isValid)
          {
            if (
              (i == 0 || !buildingFloors[floor][i - 1][j].isValid) &&
              (j == 0 || !buildingFloors[floor][i][j - 1].isValid) &&
              (i == buildingFloors[floor].Count - 1 || !buildingFloors[floor][i + 1][j].isValid) &&
              (j == buildingFloors[floor][i].Count - 1 || !buildingFloors[floor][i][j + 1].isValid)
              )
            {
              buildingFloors[floor][i][j].isValid = false;
            }
            else
            {
              cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
              cube1.transform.parent = gameObject.transform;

              cube1.transform.localScale = new Vector3(buildingFloors[floor][i][j].width, buidlingMinHeight, buildingFloors[floor][i][j].length);
              cube1.transform.position = new Vector3(buildingFloors[floor][i][j].x + buildingFloors[floor][i][j].width / 2f, buidlingMinHeight / 2f, buildingFloors[floor][i][j].y + buildingFloors[floor][i][j].length / 2f);
              cube1.GetComponent<Renderer>().material.color = Color.white;

            }
          }
        }
      }
    }
    return buildingFloors;
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
