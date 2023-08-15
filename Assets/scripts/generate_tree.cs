using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TreeBranch
{
    public TreeBranch(float xValue = 0f, float yValue = 0f, float zValue = 0f, float sizeValue = 0f)
    {
        x = xValue;
        y = yValue;
        z = zValue;
        size = sizeValue;
    }
    public float x;
    public float y;
    public float z;
    public float size;
}

public class generate_tree : MonoBehaviour
{
    private GameObject cube1, cube2;

    List<TreeBranch> sizedLineUp(float tx, float ty, float tz, float r, float calmMovement, float craziness, float freq0, float height0, float lines, float discret, float direction, float size)
    {
        List<TreeBranch> arr = new List<TreeBranch> { };
        float xMin = 0f;
        float xMax = lines;
        float ht = discret;
        for (float t = xMin; t < xMax; t += ht)
        {
            float angle = Mathf.Sin(t * freq0 + r) * (Mathf.Pow(craziness, t)) / calmMovement + direction;
            float angle2 = Mathf.Cos(t * freq0 + r) * (Mathf.Pow(craziness, t)) / calmMovement + direction;
            // float angle = Mathf.Sin(t*freq0+r)*(craziness**((r)*(Mathf.Sin(t/100)+1)/2))/calmMovement+direction;
            // float angle = t**(t/40)*Mathf.Sin(r/10+calmMovement)+direction;
            float branch = height0 + height0 / 2f * Mathf.Sin(t * 2f * Mathf.PI / 9f + 8f * Mathf.Sin(r / 6f));
            float newPosX = tx - branch * Mathf.Sin(angle) * Mathf.Sin(angle2);
            float newPosY = ty - branch * Mathf.Cos(angle);
            float newPosZ = tz - branch * Mathf.Sin(angle) * Mathf.Cos(angle2);
            if (t == xMin)
            {
                arr.Add(new TreeBranch(tx, ty, tz, size / 20f));
            }
            arr.Add(new TreeBranch(newPosX, newPosY, newPosZ, size / 20f));
            tx = newPosX;
            ty = newPosY;
            tz = newPosZ;
        }
        return arr;
    }

    float directionFunc(float tx, float ty, float i, float r)
    {
        return -(Mathf.Pow(tx % ty, 3)) / 10000f;
    }
    float calmMovementFunc(float tx, float ty, float i, float r)
    {
        return 1 / Mathf.Sin(i * 2 * Mathf.PI * Random.value);
    }
    float crazinessFunc(float tx, float ty, float i, float r)
    {
        return 1 * (Mathf.Sin(10 * ty * i) - 1);
    }
    float freqFunc(float tx, float ty, float i, float r)
    {
        return 2f * Mathf.PI + 1f * Mathf.Cos(i + 0.2f);
    }
    float heightFunc(float tx, float ty, float i, float k, float r)
    {
        return 4 / ((i + k) / 4 + 1);
    }
    float lengthFunc(float tx, float ty, float i, float r)
    {
        return 5;
    }
    float discretFunc(float tx, float ty, float i, float r)
    {
        return 1;
    }


    List<List<TreeBranch>> makeATree(float tx, float ty, float tz, int qBranch, float baseBrValue)
    {
        List<List<TreeBranch>> tree = new List<List<TreeBranch>> { };
        float length = 10f;
        int branchSegment = 0;
        float r = 1f;
        float treeDepth = 3f;
        float baseBr = baseBrValue % 1f;
        float calmMovement, craziness, freq0, height0, discret, direction, lineSize;
        int baseSegment;
        for (int i = 0; i < qBranch; i++)
        {
            if (Random.value > 0.5 || i == 0)
            {

                baseSegment = i != 0 ? (int)Mathf.Floor(tree[0].Count * baseBr) : 0;
                branchSegment = i != 0 ? (int)Mathf.Floor(baseSegment + Random.value * ((tree[0].Count - 1) - baseSegment)) : baseSegment;
                tx = i != 0 ?
                    tree[0][branchSegment].x / 2f + tree[0][branchSegment - 1].x / 2f :
                    tx;
                ty = i != 0 ?
                    tree[0][branchSegment].y / 2f + tree[0][branchSegment - 1].y / 2f :
                    ty;
                tz = i != 0 ?
                    tree[0][branchSegment].z / 2f + tree[0][branchSegment - 1].z / 2f :
                    tz;
                direction = directionFunc(tx, ty, i, r);
                calmMovement = calmMovementFunc(tx, ty, i, r);
                craziness = crazinessFunc(tx, ty, i, r);
                freq0 = freqFunc(tx, ty, i, r);
                height0 = heightFunc(tx, ty, i, 0, r);
                length = lengthFunc(tx, ty, i, r);
                discret = discretFunc(tx, ty, i, r);
                lineSize = qBranch / (i + 1);
                tree.Add(sizedLineUp(tx, ty, tz, r, calmMovement, craziness, freq0, height0, length, discret, direction, lineSize));
            }
        }
        float curTreeLenght = tree.Count;
        for (int j = 0; j < treeDepth; j++)
        {
            curTreeLenght = tree.Count;
            for (int i = 0; i < curTreeLenght; i++)
            {
                for (int k = 0; k < treeDepth; k++)
                {
                    if (i == 0)
                    {
                        baseSegment = (int)Mathf.Floor(tree[0].Count * baseBr);
                        branchSegment = (int)Mathf.Floor(baseSegment + Random.value * ((tree[0].Count - 1) - baseSegment));
                    }
                    else
                    {
                        branchSegment = (int)Mathf.Floor(Random.value * (tree[i].Count - 1)) + 1;
                    }
                    tx = tree[i][branchSegment].x / 2f + tree[i][branchSegment - 1].x / 2f;
                    ty = tree[i][branchSegment].y / 2f + tree[i][branchSegment - 1].y / 2f;
                    tz = tree[i][branchSegment].z / 2f + tree[i][branchSegment - 1].z / 2f;
                    direction = directionFunc(tx, ty, i, r);
                    calmMovement = calmMovementFunc(tx, ty, i, r);
                    craziness = crazinessFunc(tx, ty, i, r);
                    freq0 = freqFunc(tx, ty, i, r);
                    height0 = heightFunc(tx, ty, i, k, r);
                    length = lengthFunc(tx, ty, i, r);
                    discret = discretFunc(tx, ty, i, r);
                    lineSize = treeDepth * treeDepth * qBranch / (k + j * curTreeLenght + i * treeDepth + 1f);
                    tree.Add(sizedLineUp(tx, ty, tz, r, calmMovement, craziness, freq0, height0, length, discret, direction, lineSize));
                }
            }
        }
        // console.log(tree[0]);
        return tree;
    }

    void drawLineUpPos(List<TreeBranch> coords, float x, float y, float z, float r, float lineSize, float scr)
    {
        float lineWidth = 1f;
        for (var i = 1; i < coords.Count; i += 1)
        {
            if (coords[i].size == 0f)
            {
                lineWidth = 1f;
            }
            else
            {
                lineWidth = lineSize / 50f * coords.Count / (i) / 10f;
            }
            Vector3 branchStart = new Vector3(scr * coords[i - 1].x + x, scr * coords[i - 1].y + y, scr * coords[i - 1].z + z);
            Vector3 branchEnd = new Vector3(scr * coords[i].x + x, scr * coords[i].y + y, scr * coords[i].z + z);
            float branchPointsDistance = Vector3.Distance(branchStart, branchEnd);
            Vector3 branchSize = new Vector3(lineWidth, lineWidth, branchPointsDistance);

            cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.parent = gameObject.transform;
            cube1.transform.localScale = branchSize;
            cube1.transform.position = branchStart;
            cube1.transform.LookAt(branchEnd);
            cube1.transform.position = new Vector3((branchEnd.x + branchStart.x) / 2f ,(branchEnd.y + branchStart.y) / 2f, (branchEnd.z + branchStart.z) / 2f);
            cube1.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    void drawTree(List<List<TreeBranch>> b, float x, float y, float z)
    {
        float scr = -1f;
        float lineSize = 0.1f;
        for (int i = 0; i < b.Count; i++)
        {
            lineSize = Mathf.Sqrt(-i + b.Count) * 1.5f;
            drawLineUpPos(b[i], x, y, z, 0f, lineSize, scr);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<List<TreeBranch>> b = makeATree(0f, 0.1f, 0f, 3, 0.3f);
        drawTree(b,gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
