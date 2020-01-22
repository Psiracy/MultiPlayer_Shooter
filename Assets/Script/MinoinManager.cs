using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoinManager : MonoBehaviour
{
    [SerializeField]
    GameObject minoin, player;
    List<Minoin> minoins;
    // Start is called before the first frame update
    void Start()
    {
        minoins = new List<Minoin>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int baseMinion = 0; baseMinion < minoins.Count; baseMinion++)
        {
            Minoin currentMinion = minoins[baseMinion];
            for (int minionToCheckWith = 0; minionToCheckWith < minoins.Count; minionToCheckWith++)
            {
                if (currentMinion != minoins[minionToCheckWith])
                {
                    if (Vector3.Distance(currentMinion.transform.position, minoins[minionToCheckWith].transform.position) < .8f)
                        currentMinion.transform.position = Vector3.MoveTowards(currentMinion.transform.position, new Vector3(minoins[minionToCheckWith].transform.position.x, currentMinion.transform.position.y, minoins[minionToCheckWith].transform.position.z), Time.deltaTime * -1);
                }
            }
        }
    }

    public void MakeMinoins()
    {
        if (minoins.Count >= 12)
        {
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject minionGo = Instantiate(minoin, new Vector3(player.transform.position.x, minoin.transform.position.y, player.transform.position.z) + new Vector3(Random.Range(1.5f, 4), 0, Random.Range(1.5f, 4)), Quaternion.identity);
            Minoin minionScript = minionGo.GetComponent<Minoin>();
            minionScript.player = player;
            minoins.Add(minionScript);
        }
    }
    public int GetMinoinsCount()
    {
        return minoins.Count;
    }
    public List<Minoin> GetMinoins()
    {
        return minoins;
    }
}
