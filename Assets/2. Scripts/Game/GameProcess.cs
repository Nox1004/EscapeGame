using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    // Note. GameManager랑 겹칠 것으로 예상이 된다.
    public class GameProcess : PersistantSingleton<GameProcess>
    {
        [SerializeField] LevelDesign cntLevelDesign;


    }
}