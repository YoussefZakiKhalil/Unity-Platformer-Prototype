using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Scripts/ScriptableObjecs/BallDesigns", menuName = "ScriptableObjects/BallDesign")]
public class BallDesign : ScriptableObject

{
    public string designName;
    public Material ball;
    public Material rings;

}
