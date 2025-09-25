using UnityEngine;
using Fusion;
/// <summary>
/// Ball class that moves forward and despawns after 5 seconds
/// </summary>
/// https://doc.photonengine.com/ja-jp/fusion/current/tutorials/host-mode-basics/3-prediction#:~:text=Ball%E3%83%97%E3%83%AC%E3%83%8F%E3%83%96-,%E4%BA%88%E6%B8%AC%E5%8B%95%E4%BD%9C,-%E7%9B%AE%E6%A8%99%E3%81%AF%E3%80%81%E5%85%A8%E3%81%A6
public class Ball : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }

    public void Init()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += 5 * transform.forward * Runner.DeltaTime;
    }
}
