

using System;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using UnityEngine;


[CreateAssetMenu(fileName = "FleetConnectionSetting_Client", menuName = "ScriptableObjects/FleetConnectionSetting_Client")]
public class FleetConnectionSetting_Client : ScriptableObject
{
    [SerializeField] private string fleetID;
    [SerializeField] private string _IAMARN;

    public string FleetID => new (fleetID);

    public string IAMARN => new (_IAMARN);
}
