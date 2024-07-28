using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _prevCamera;
    [SerializeField] CinemachineVirtualCamera _camNeedToSwitch;
    public Transform pos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            CameraController.Instance.SwitchingCamera(_camNeedToSwitch);
            _prevCamera.Follow = collision.transform;
           
            GameManager.Instance.pos=pos.position;
            StartCoroutine(TransitionCamera(collision));
        }

    }
    IEnumerator TransitionCamera(Collider2D player)
    {

        GameManager.Instance.transitionAnim.gameObject.SetActive(true);
        GameManager.Instance.transitionAnim.SetBool("Start", true);
        GameManager.Instance.transitionAnim.SetBool("End", false);
        yield return new WaitForSeconds(1.5f);

        
        CameraController.Instance.SwitchingCamera(_camNeedToSwitch);
        yield return new WaitForSeconds(1f);
        player.GetComponent<Player>().ResetData(GameManager.Instance.pos);
        yield return new WaitForSeconds(1f);
       
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.transitionAnim.SetBool("Start", false);
        GameManager.Instance.transitionAnim.SetBool("End", true);
        yield return new WaitForSeconds(2f);
        player.GetComponent<PlayerInput>().enabled = true;
        GameManager.Instance.transitionAnim.gameObject.SetActive(false);


    }
}
