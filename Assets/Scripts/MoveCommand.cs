using UnityEngine;

public abstract class MoveCommand
{
    protected Snake snake;

    public MoveCommand(Snake snake) {
        this.snake = snake;
    }

    public abstract void Execute();
}

public class UpCommand : MoveCommand {

    public UpCommand(Snake snake) : base(snake){}
    public override void Execute()
    {
        if (snake.getDirection() != Vector2.down && !snake.getDirectionKeyUpdated()) {
            snake.setDirection(Vector2.up);
            snake.setDirectionUpdated();
        }
    }
}

public class RightCommand : MoveCommand {

    public RightCommand(Snake snake) : base(snake){}

    public override void Execute()
    {
        if (snake.getDirection() != Vector2.left && !snake.getDirectionKeyUpdated()) {
            snake.setDirection(Vector2.right);
            snake.setDirectionUpdated();
        }
    }
}

public class DownCommand : MoveCommand {

    public DownCommand(Snake snake) : base(snake){}

    public override void Execute()
    {
        if (snake.getDirection() != Vector2.up && !snake.getDirectionKeyUpdated()) {
            snake.setDirection(Vector2.down);
            snake.setDirectionUpdated();
        }
    }
}

public class LeftCommand : MoveCommand {
    
    public LeftCommand(Snake snake) : base(snake){}

    public override void Execute()
    {
        if (snake.getDirection() != Vector2.right && !snake.getDirectionKeyUpdated()) {
            snake.setDirection(Vector2.left);
            snake.setDirectionUpdated();
        }
    }
}