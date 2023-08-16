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
  public Vector2 areaSize = new Vector2(50f, 50f);
  public float buidlingMinHeight = 5f;
  public int floors = 5;
  public float cellsPerRow = 10f;
  public int texuresAmount = 4;
  private Texture2D m_MainTexture;

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
              GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
              cube.transform.parent = gameObject.transform;

              cube.transform.localScale = new Vector3(buildingFloors[floor][i][j].width, buidlingMinHeight, buildingFloors[floor][i][j].length);
              cube.transform.position = new Vector3(
                buildingFloors[floor][i][j].x + buildingFloors[floor][i][j].width / 2f,
                buidlingMinHeight / 2f * (2 * floor + 1),
                buildingFloors[floor][i][j].y + buildingFloors[floor][i][j].length / 2f
              );
              cube.GetComponent<Renderer>().material.color = Color.white;

              int textureIndex = (int)Mathf.Round(Random.value * (texuresAmount - 1));
              m_MainTexture = Resources.Load<Texture2D>("generated_building/wall/brick_wall" + textureIndex);

              cube.GetComponent<Renderer>().material.SetTexture("_MainTex", m_MainTexture);
              cube.GetComponent<Renderer>().material.SetFloat("_Mode", 3);
              cube.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
              cube.GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
              cube.GetComponent<Renderer>().material.SetInt("_ZWrite", 1);
              cube.GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
              cube.GetComponent<Renderer>().material.DisableKeyword("_ALPHABLEND_ON");
              cube.GetComponent<Renderer>().material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
              cube.GetComponent<Renderer>().material.renderQueue = 3000;

              
              GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
              roof.transform.parent = gameObject.transform;

              roof.transform.localScale = new Vector3(buildingFloors[floor][i][j].width + 0.5f, 0.5f, buildingFloors[floor][i][j].length + 0.5f);
              roof.transform.position = new Vector3(
                buildingFloors[floor][i][j].x + buildingFloors[floor][i][j].width / 2f,
                buidlingMinHeight * (floor + 1),
                buildingFloors[floor][i][j].y + buildingFloors[floor][i][j].length / 2f
              );
              roof.GetComponent<Renderer>().material.color = Color.white;

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
