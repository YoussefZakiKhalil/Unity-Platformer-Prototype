using CUAS.MMT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectMaterialCommand : ICommandU
{
    DesignerController dc;

    Material oldMaterial = null;
    Material newMaterial = null;
    bool checkBallorRingButton = true;
    public selectMaterialCommand(DesignerController dc_, Material material, bool checkBallorRingButton_)
    {
        dc = dc_;
        checkBallorRingButton = checkBallorRingButton_;
        if  (checkBallorRingButton  ==  true)
            oldMaterial = dc.bdIObject.GetComponent<MeshRenderer>().material;
        else
            oldMaterial  = dc.bdIObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        newMaterial = material;

    }
    public void Execute()
    {
        if (checkBallorRingButton == true)
        {
            dc.bdIObject.GetComponent<MeshRenderer>().material = newMaterial;
        }
        else
        {
            for (int idx = 0; idx < dc.bdIObject.transform.childCount; idx++)
            {
                dc.bdIObject.transform.GetChild(idx).gameObject.GetComponent<MeshRenderer>().material = newMaterial;
            }
        }
    }

    public void Undo()
    {
        if (checkBallorRingButton == true)
        {
            dc.bdIObject.GetComponent<MeshRenderer>().material = oldMaterial;
        }
        else
        {
            for (int idx = 0; idx < dc.bdIObject.transform.childCount; idx++)
            {
                dc.bdIObject.transform.GetChild(idx).gameObject.GetComponent<MeshRenderer>().material = oldMaterial;
            }
        }
    }
}
