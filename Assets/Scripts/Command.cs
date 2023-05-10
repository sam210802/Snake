using UnityEngine;

public abstract class Command {
    public abstract void Execute(bool keyDown);
}

public abstract class MoveCommand : Command
{
    protected Snake snake;

    public MoveCommand(Snake snake) {
        this.snake = snake;
    }
}

public class UpCommand : MoveCommand {

    public UpCommand(Snake snake) : base(snake){}
    public override void Execute(bool keyDown)
    {
        if (keyDown) {
            if (snake.getDirection() != Vector2.down && !snake.getDirectionKeyUpdated()) {
                snake.setDirection(Vector2.up);
                snake.setDirectionUpdated();
            }
        }
    }
}

public class RightCommand : MoveCommand {

    public RightCommand(Snake snake) : base(snake){}

    public override void Execute(bool keyDown)
    {
        if (keyDown) {
            if (snake.getDirection() != Vector2.left && !snake.getDirectionKeyUpdated()) {
                snake.setDirection(Vector2.right);
                snake.setDirectionUpdated();
            }
        }
    }
}

public class DownCommand : MoveCommand {

    public DownCommand(Snake snake) : base(snake){}

    public override void Execute(bool keyDown)
    {
        if (keyDown) {
            if (snake.getDirection() != Vector2.up && !snake.getDirectionKeyUpdated()) {
                snake.setDirection(Vector2.down);
                snake.setDirectionUpdated();
            }
        }
    }
}

public class LeftCommand : MoveCommand {
    
    public LeftCommand(Snake snake) : base(snake){}

    public override void Execute(bool keyDown)
    {
        if (keyDown) {
            if (snake.getDirection() != Vector2.right && !snake.getDirectionKeyUpdated()) {
                snake.setDirection(Vector2.left);
                snake.setDirectionUpdated();
            }
        }
    }
}

public abstract class GameCommand : Command
{
    protected GameManager gm;

    public GameCommand(GameManager gm) {
        this.gm = gm;
    }
}

public class PauseCommand : GameCommand {

    public PauseCommand(GameManager gm) : base(gm){}
    public override void Execute(bool keyDown)
    {
        if (!keyDown) {
            if (gm.isGamePaused()) gm.resumeGame();
            else gm.pauseGameGUI();
        }
    }
}

public class RewindCommand : GameCommand {

    public RewindCommand(GameManager gm) : base(gm){}
    public override void Execute(bool keyDown)
    {
        if (keyDown) {
            if (gm.getNumUpdates() > 0) gm.startRewinding();
        } else {
            gm.stopRewinding();
        }
    }
}