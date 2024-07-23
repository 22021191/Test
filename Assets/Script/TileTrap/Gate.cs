using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.PlayerSettings;



public class Gate : MonoBehaviour
{
    [SerializeField] private Transform nextZoomPos;
    [SerializeField] private CinemachineVirtualCamera nextCam;
    bool isTransition = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player"&&GameManager.Instance.key)
        {
            collision.gameObject.GetComponent<Player>().ResetData(nextZoomPos.position);
            GameManager.Instance.pos = nextZoomPos.position;
            StartCoroutine(TransitionCamera(collision));
        }
    }
    IEnumerator TransitionCamera(Collision2D player)
    {

        GameManager.Instance.transitionAnim.gameObject.SetActive(true);
        GameManager.Instance.transitionAnim.SetBool("Start", true);
        GameManager.Instance.transitionAnim.SetBool("End", false);

        yield return new WaitForSeconds(1f);
        GameManager.Instance.endGame.SetActive(true );
        Destroy(gameObject);


    }

}
