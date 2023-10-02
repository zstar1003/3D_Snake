using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    // 设置
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;
    public float bodySpeed = 5f;
    public int Gap = 10;

    // 预制体
    public GameObject bodyPrefab;  //身体组件

    // 身体组件集合
    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionHistory = new List<Vector3>();

    //音乐控制器
    public AudioController audioController; 

    private void Start()
    {
        addBodyPart();
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }
    private void Update()
    {
        // 向前移动
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // 方向操控
        float steerDirection = Input.GetAxis("Horizontal");  // 返回值从 -1 到 1
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        // 保存位置移动史
        _positionHistory.Insert(0, transform.position);

        // 移动身体组件
        int index = 0;
        foreach (var body in _bodyParts)
        {
            Vector3 point = _positionHistory[Mathf.Clamp(index * Gap, 0, _positionHistory.Count - 1)];

            // 让贪吃蛇的身体组件沿着头部的移动轨迹运动
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;

            // 让身体组件朝向头部移动的方向 
            body.transform.LookAt(point);

            index++;
        }
    }
       
    // 初始三个body无标签
    private void addBodyPart()
    {
        GameObject body = Instantiate(bodyPrefab, new Vector3(0, transform.position.y, 0), Quaternion.identity);
        _bodyParts.Add(body);
    }

    // 后续添加的body打上Block标签
    private void addBodyPart_Block()
    {
        GameObject body = Instantiate(bodyPrefab, new Vector3(0, _bodyParts.Last().transform.position.y, 0), Quaternion.identity);
        body.tag = "Block";
        _bodyParts.Add(body);
    }


    //触发检测
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {   
            //必须先删除，否则会导致多次触发
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