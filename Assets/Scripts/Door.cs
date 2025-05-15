using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Properties;
using TMPro;
// Created by Saif Al-Temmemi
// Edited by Herman Bergström
public class Door : MonoBehaviour, InteractableObject
{
    private readonly float[] openRotations = { 90f, -90f };
    [SerializeField] private GameObject model;
    private float t = 0f;
    [HideInInspector] public State state = State.Closed;
    private int openingId;
    [SerializeField] private LayerMask playerMask;
    private readonly float speed = 1.5f;
    [SerializeField] private TextMeshProUGUI interactionText;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (state == State.Opening)
        {
            t += Time.deltaTime * speed;
            t = Mathf.Clamp(t, 0f, 1f);
            model.transform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, openRotations[openingId], t), 0f);
            if (t == 1f)
            {
                state = State.Open;
            }
        }
        else if (state == State.Closing)
        {
            t += Time.deltaTime * speed;
            t = Mathf.Clamp(t, 0f, 1f);
            model.transform.localRotation =  Quaternion.Euler(0f, Mathf.Lerp(openRotations[openingId], 0f, t), 0f);
            
            if (t == 1f)
            {
                state = State.Closed;
            }
        }

        if (state == State.Closed || state == State.Open)
        {
            t = 0f;
        }
    }

    public void InteractRangeEnter()
    {
        interactionText.text = "Press F to\n" + (state == Door.State.Closed ? "open" : "close") + " gate";
    }

    public void InteractRangeStay()
    {
        interactionText.text = "Press F to\n" + (state == Door.State.Closed ? "open" : "close") + " gate";
    }

    public void InteractRangeExit()
    {
        interactionText.text = string.Empty;
    }
    
    public void Interact()
    {
        if (state == State.Closed || state == State.Open)
        {
            state = state == State.Closed ? State.Opening : state == State.Open ? State.Closing : state;
            if (state == State.Opening)
            {
                SetOpeningId(player.transform.position);
            }
        }
        InteractRangeEnter();
    }

    public void SetOpeningId(Vector3 playerPos)
    {
        Vector3 localPos = model.transform.localPosition;
        bool result = Physics.CheckBox(model.transform.TransformPoint(new Vector3(localPos.x, localPos.y, localPos.z - 3f)), Vector3.one * 1.5f, transform.rotation, playerMask);
        if (result)
        {
            openingId = 0;
            return;
        }
        result = Physics.CheckBox(model.transform.TransformPoint(new Vector3(localPos.x, localPos.y, localPos.z + 3f)), Vector3.one * 1.5f, transform.rotation, playerMask);
        if (result)
        {
            openingId = 1;
            return;
        }
    }

    public enum State
    {
        Closed,
        Closing,
        Open,
        Opening
    }
    
}
