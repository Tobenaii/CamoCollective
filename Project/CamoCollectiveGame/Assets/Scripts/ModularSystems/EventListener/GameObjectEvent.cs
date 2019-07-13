using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularSystem/Event/GameObject")]
public class GameObjectEvent : GameEvent<CustomGameObjectEvent, GameObjectEvent, GameObject>
{

}
