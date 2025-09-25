using Fusion;
using UnityEngine;
/// <summary>
/// Input structure used by the NetworkCharacterControllerPrototype
/// </summary>
/// <https://doc.photonengine.com/ja-jp/fusion/current/tutorials/host-mode-basics/2-setting-up-a-scene#:~:text=%E3%81%A6%E3%81%8F%E3%81%A0%E3%81%95%E3%81%84%E3%80%82-,%E5%85%A5%E5%8A%9B%E3%81%AE%E5%8F%8E%E9%9B%86,-%E5%85%A5%E5%8A%9B%E6%A8%A9%E9%99%90%E3%82%92>
public struct NetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON0 = 1;

    public NetworkButtons buttons;
    public Vector3 direction;
}
