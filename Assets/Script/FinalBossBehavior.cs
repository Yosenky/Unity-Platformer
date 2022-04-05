﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehavior : MonoBehaviour
{

  public GameObject cactusBulletStage1; //Bullets shot out of cactus during stage 1
  public EdgeCollider2D edgeCollider; //Collider inside of the boss for the sake of spawning spikes inside of him

  public GameObject[] cactusBulletStage1Array; //Holds the cactus bullets once they have been instantiated
  public float[] cactusBulletStage1RotatonArray; //Holds the cactus bullets' rotation
  public Rigidbody2D[] cactusBulletStage1RigidBodyArray; //Holds the cactus bullets' rigidbodies

  public SpriteRenderer[] spriteRenderers; //Sprite renderers, used to swap sprites
  public Sprite deletedBullets; //The dust particles for bullets when they are destroyed

  int numberOfBullets; //Number of bullets
  bool bulletsDestroyed = true; //False if bullets need to be destroyed, True if already destroyed


    void Start()
    {
        stageOneShoot();
    }

    void Update()
    {
        testingShoot();
    }

    //Shooting script for stage 1
    void stageOneShoot()
    {
      //Instantiate bullets around cactus
      numberOfBullets = Random.Range(5,8);
      //int numberOfBullets = 8; //Static number used in bug testing

      //initializing arrays
      cactusBulletStage1Array = new GameObject[numberOfBullets];
      cactusBulletStage1RotatonArray = new float[numberOfBullets];
      cactusBulletStage1RigidBodyArray = new Rigidbody2D[numberOfBullets];
      spriteRenderers = new SpriteRenderer[numberOfBullets];

      StartCoroutine(spawnNewBullets());



      //Pause
      //Shoot slowly out from spawn
    }

    IEnumerator spawnNewBullets(){
      for(int i = 0; i < numberOfBullets; i++)
      {
        float bulletRotation = 90 - (180/(numberOfBullets-1)) * i; //Rotation of the bullets as they are spawned in, creates a semicircle around cactus
        if(!bulletsDestroyed)
        {
          changeBulletSprites();
          yield return new WaitForSeconds(1);
          destroyBullets();
          bulletsDestroyed = true;
        }
        cactusBulletStage1Array[i] = Instantiate(cactusBulletStage1, new Vector3(edgeCollider.transform.position.x, edgeCollider.transform.position.y, 0),Quaternion.Euler(0, 0, bulletRotation));
        spriteRenderers[i] = cactusBulletStage1Array[i].GetComponent<SpriteRenderer>();
        cactusBulletStage1RotatonArray[i] = bulletRotation;
        cactusBulletStage1RigidBodyArray[i] = cactusBulletStage1Array[i].GetComponent<Rigidbody2D>();
      }
    }
    
    void changeBulletSprites()
    {
      for(int j = 0; j < spriteRenderers.Length; j++)
      {
        spriteRenderers[j].sprite = deletedBullets;
      }
    }

    void destroyBullets()
    {
      for(int i = 0; i < numberOfBullets; i++)
      {
        Destroy(cactusBulletStage1Array[i]);
      }      
    }

    void testingShoot()
    { 
      int movementSpeed = 50;
      int force = 20;
      //Shoots bullets(will automate later)
      if(Input.GetKeyDown(KeyCode.Space))
      {
        for(int i = 0; i < cactusBulletStage1Array.Length; i++)
        {
          cactusBulletStage1RigidBodyArray[i].constraints = RigidbodyConstraints2D.None;
          cactusBulletStage1RigidBodyArray[i].velocity = new Vector2(-Mathf.Sin(cactusBulletStage1RigidBodyArray[i].transform.rotation.z) * 10,
                                                                     Mathf.Cos(cactusBulletStage1RigidBodyArray[i].transform.rotation.z) * 10);
        }

      }

      if(Input.GetKeyDown(KeyCode.F)){
        bulletsDestroyed = false;
        StartCoroutine(spawnNewBullets());
      }

    }
}
