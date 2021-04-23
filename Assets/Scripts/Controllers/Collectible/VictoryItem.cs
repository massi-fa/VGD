
public class VictoryItem : CollectibleController
{
    protected override void ActionPerformed(GameCharacterController gameCharacterController)
    {
        base.ActionPerformed(gameCharacterController);
        SceneTransitioner.GetInstance().GoToScene(3);
    }
}
