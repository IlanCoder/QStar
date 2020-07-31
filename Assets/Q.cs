using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q : MonoBehaviour {
    float alpha;
    float gamma;
    float currentQ;
    float[] allQs;
    float rho;

    void Start() {
        allQs = new float[100];
        alpha = 0.4f;
        gamma = 0.2f;
        currentQ = 0;
        for(int iteration = 0; iteration < 100; ++iteration) {
            allQs[iteration] = currentQ;
            Debug.Log(currentQ);
            rho = Random.Range(0, 100) / 100;            
            currentQ = (1 - alpha) * currentQ + alpha*(rho + gamma + CheckMax());           
        }      
    }

    float CheckMax() {
        float max = allQs[0];
        for(int qListPos = 1; qListPos < allQs.Length; ++qListPos) {
            if (max < allQs[qListPos]) {
                max = allQs[qListPos];
            }
        }
        return max;
    }
}
