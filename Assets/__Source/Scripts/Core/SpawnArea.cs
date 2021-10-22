using UnityEngine;

public class SpawnArea : MonoBehaviour
{
#if UNITY_EDITOR
    public enum Type { HalfLeft, HalfRight, HalfTop, Custom, Full }
    public Vector2 CustomStartFinish = new Vector2(0, 90);
    public Type CircleType = Type.Full;
    public float resolution = 10;
    public float spawnRadius = 5f;
    public bool visualise = true;

    public string PointDistance = "0";

    private bool spawn = false;
    public void Spawn()
    {
        spawn = true;
    }

    private void OnDrawGizmos()
    {
        if (!visualise || Application.isPlaying)
            return;
        Vector2 custom = CustomStartFinish / 180*Mathf.PI;
        float theta = CircleType == Type.Custom ? custom.x : 0;
        float x = spawnRadius * Mathf.Cos(theta);
        float y = spawnRadius * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, y, 0);
        Vector3 newpos = pos;

        float delta = 1 / resolution;

        int i = 0;

        switch (CircleType)
        {
            case Type.HalfLeft:


                break;
            case Type.HalfRight:

                break;

            case Type.HalfTop:
                for (theta = delta; theta - delta < Mathf.PI; theta += delta)
                {
                    x = spawnRadius * Mathf.Cos(theta);
                    y = spawnRadius * Mathf.Sin(theta);
                    newpos = transform.position + new Vector3(x, y, 0);
                    Gizmos.DrawLine(pos, newpos);
                    pos = newpos;

                    if (spawn)
                    {
                        Instantiate(new GameObject { name = "Point" + i.ToString() }, pos, Quaternion.identity);
                    }
                    i++;
                }

                spawn = false;
                break;

            case Type.Full:

                for (theta = delta; theta - delta < Mathf.PI * 2; theta += delta)
                {
                  
                    x = spawnRadius * Mathf.Cos(theta);
                    y = spawnRadius * Mathf.Sin(theta);
                    newpos = transform.position + new Vector3(x, y, 0);
                    Gizmos.DrawLine(pos, newpos);
                    pos = newpos;
                    if (spawn)
                    {
                        Instantiate(new GameObject { name = "Point" + i.ToString() }, pos, Quaternion.identity);
                    }

                    i++;
                }
                spawn = false;
                break;

            case Type.Custom:
                float avg = 0;
                for (theta = custom.x; theta < custom.y; theta += delta)
                {
                    x = spawnRadius * Mathf.Cos(theta);
                    y = spawnRadius * Mathf.Sin(theta);
                    newpos = transform.position + new Vector3(x, y, 0);
                    Gizmos.DrawLine(pos, newpos);
                    avg += Vector3.Distance(pos, newpos);
                    pos = newpos;
                    if (spawn)
                    {
                        GameObject g = new GameObject { name = "Point" + i.ToString() };
                        g.transform.position = pos;
                    }

                    Gizmos.DrawWireSphere(pos, 0.1f);

                    i++;
                }

                avg /= i;

                PointDistance = avg.ToString();

                spawn = false;
                break;

            default:
                break;
        }
    }
#endif
}
