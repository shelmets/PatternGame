using System;
using System.Collections.Generic;

namespace PatternGame
{
    interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }
    class OneMoveCommand : ICommand
    {
        private Battlefield battlefield;
        private Army firstBeforeMove;
        private Army secondBeforeMove;
        private Army firstAfterMove;
        private Army secondAfterMove;
        public OneMoveCommand(Battlefield field)
        {
            battlefield = field;
        }
        public void Execute()
        {
            firstBeforeMove = battlefield.FirstArmy.GetSnapshot();
            secondBeforeMove = battlefield.SecondArmy.GetSnapshot();
            battlefield.Move();
            firstAfterMove = battlefield.FirstArmy.GetSnapshot();
            secondAfterMove = battlefield.SecondArmy.GetSnapshot();
        }

        public void Undo()
        {
            battlefield.FirstArmy = firstBeforeMove;
            battlefield.SecondArmy = secondBeforeMove;
        }

        public void Redo()
        {
            battlefield.FirstArmy = firstAfterMove;
            battlefield.SecondArmy = secondAfterMove;
        }
    }
    class PlayToEndCommand: ICommand
    {
        private Battlefield battlefield;
        private Army firstBeforeMove;
        private Army secondBeforeMove;
        private Army firstAfterMove;
        private Army secondAfterMove;
        public PlayToEndCommand(Battlefield field)
        {
            battlefield = field;
        }
        public void Execute()
        {
            firstBeforeMove = battlefield.FirstArmy.GetSnapshot();
            secondBeforeMove = battlefield.SecondArmy.GetSnapshot();
            battlefield.PlayToTheEnd();
            firstAfterMove = battlefield.FirstArmy.GetSnapshot();
            secondAfterMove = battlefield.SecondArmy.GetSnapshot();
        }

        public void Undo()
        {
            battlefield.FirstArmy = firstBeforeMove;
            battlefield.SecondArmy = secondBeforeMove;
        }

        public void Redo()
        {
            battlefield.FirstArmy = firstAfterMove;
            battlefield.SecondArmy = secondAfterMove;
        }
    }
    class CommandInvoker
    {
        private Stack<ICommand> StackUndo = new Stack<ICommand>();
        private Stack<ICommand> StackRedo = new Stack<ICommand>();
        private Battlefield battlefield;

        public CommandInvoker(Battlefield field)
        {
            this.battlefield = field;
        }

        public void Invoke(ICommand cmd)
        {
            cmd.Execute();
            StackUndo.Push(cmd);
            StackRedo.Clear();
        }

        public void Undo()
        {
            ICommand cmd = StackRedo.Pop();
            cmd.Undo();
            StackRedo.Push(cmd);
        }

        public void Redo()
        {
            ICommand cmd = StackRedo.Pop();
            cmd.Redo();
            StackRedo.Push(cmd);
        }

        public void Move()
        {
            Invoke(new OneMoveCommand(battlefield));
        }

        public void PlayToTheEnd()
        {
            Invoke(new PlayToEndCommand(battlefield));
        }
    }
}
