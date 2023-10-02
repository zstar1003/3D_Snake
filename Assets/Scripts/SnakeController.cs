using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    // ����
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;
    public float bodySpeed = 5f;
    public int Gap = 10;

    // Ԥ����
    public GameObject bodyPrefab;  //�������

    // �����������
    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionHistory = new List<Vector3>();

    //���ֿ�����
    public AudioController audioController; 

    private void Start()
    {
        addBodyPart();
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }
    private void Update()
    {
        // ��ǰ�ƶ�
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // ����ٿ�
        float steerDirection = Input.GetAxis("Horizontal");  // ����ֵ�� -1 �� 1
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        // ����λ���ƶ�ʷ
        _positionHistory.Insert(0, transform.position);

        // �ƶ��������
        int index = 0;
        foreach (var body in _bodyParts)
        {
            Vector3 point = _positionHistory[Mathf.Clamp(index * Gap, 0, _positionHistory.Count - 1)];

            // ��̰���ߵ������������ͷ�����ƶ��켣�˶�
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;

            // �������������ͷ���ƶ��ķ��� 
            body.transform.LookAt(point);

            index++;
        }
    }
       
    // ��ʼ����body�ޱ�ǩ
    private void addBodyPart()
    {
        GameObject body = Instantiate(bodyPrefab, new Vector3(0, transform.position.y, 0), Quaternion.identity);
        _bodyParts.Add(body);
    }

    // ������ӵ�body����Block��ǩ
    private void addBodyPart_Block()
    {
        GameObject body = Instantiate(bodyPrefab, new Vector3(0, _bodyParts.Last().transform.position.y, 0), Quaternion.identity);
        body.tag = "Block";
        _bodyParts.Add(body);
    }


    //�������
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {   
            //������ɾ��������ᵼ�¶�δ���
            Destroy(other.gameObject);
            addBodyPart_Block();
            GameObject.Find("SpawnPoint").GetComponent<SpawnItem>().SpawnItems();
            audioController.PlaySfx(audioController.eat);
        }
        else if (other.tag == "Block")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}