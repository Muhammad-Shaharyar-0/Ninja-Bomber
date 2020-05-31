using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

	public static int RoundToInt(float f)
    {       
            return Mathf.RoundToInt(f);
    } 


    public static int RandomEven(int min, int maxExclusive)
    {
        int value = Random.Range(min, maxExclusive + 1);

        while(value % 2 != 0)
        {
            value = Random.Range(min, maxExclusive + 1);
        }

        return value;
    }

    public static int RandomOdd(int min, int maxExclusive)
    {
        int value = Random.Range(min, maxExclusive + 1);

        while (value % 2 == 0)
        {
            value = Random.Range(min, maxExclusive + 1);
        }

        return value;
    }

    public static Vector3 RoundedVector3(Vector3 pos)
    {
        Vector3 roundedV3 = new Vector3(RoundToInt(pos.x), RoundToInt(pos.y), RoundToInt(pos.z));

        return roundedV3;
    }

}
