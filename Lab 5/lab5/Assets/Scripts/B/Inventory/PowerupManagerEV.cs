using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    GREENMUSHROOM = 0,
    REDMUSHROOM = 1
}

public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    // Start is called before the first frame update
    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++) {
                Powerup p = powerupInventory.Get(i);
                if (p != null) {
                    AddPowerupUI(i, p.powerupTexture);
                } else {
                    powerupIcons[i].SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }
        
    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }

    public void AttemptConsumePowerup(KeyCode k) {
        int inventoryIndex = 0;
        Powerup p = default(Powerup);
        bool found = false;
        List<int> attributes;
        int absoluteSpeedBooster;
        int absoluteJumpBooster;
        switch (k) {
            case KeyCode.Z:
                for (int i = 0; i < powerupInventory.Items.Count; i++) {
                    p = powerupInventory.Get(i);
                    if (p != null && p.index == PowerupIndex.GREENMUSHROOM) {
                        found = true;
                        inventoryIndex = i;
                        break;
                    }
                }
                if (found) {
                    powerupInventory.Remove(inventoryIndex);
                    powerupIcons[inventoryIndex].SetActive(false);
                    attributes = p.Utilise();
                    absoluteSpeedBooster = attributes[0];
                    absoluteJumpBooster = attributes[1];
                    consume(absoluteSpeedBooster, absoluteJumpBooster);
                } else {
                    Debug.Log("No GreenMushroom in inventory.");
                }
                break;
            case KeyCode.X:
                for (int i = 0; i < powerupInventory.Items.Count; i++) {
                    p = powerupInventory.Get(i);
                    if (p != null && p.index == PowerupIndex.REDMUSHROOM) {
                        found = true;
                        inventoryIndex = i;
                        break;
                    }
                }
                if (found) {
                    powerupInventory.Remove(inventoryIndex);
                    powerupIcons[inventoryIndex].SetActive(false);
                    attributes = p.Utilise();
                    absoluteSpeedBooster = attributes[0];
                    absoluteJumpBooster = attributes[1];
                    consume(absoluteSpeedBooster, absoluteJumpBooster);
                } else {
                    Debug.Log("No RedMushroom in inventory.");
                }
                break;
        }
    }

    public void consume(int aboluteSpeedBooster, int aboluteJumpBooster) {
		marioMaxSpeed.ApplyChange(aboluteSpeedBooster);
        marioJumpSpeed.ApplyChange(aboluteJumpBooster);
		StartCoroutine(removeEffect(aboluteSpeedBooster, aboluteJumpBooster));
	}

    IEnumerator removeEffect(int aboluteSpeedBooster, int aboluteJumpBooster) {
		yield return new WaitForSeconds(5.0f);
        marioMaxSpeed.ApplyChange(-1 * aboluteSpeedBooster);
        marioJumpSpeed.ApplyChange(-1 * aboluteJumpBooster);
	}

    public void OnApplicationQuit()
    {
        //ResetValues();
        powerupInventory.Clear();
    }
}
