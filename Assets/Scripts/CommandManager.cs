using System;
using System.Collections.Generic;
using UnityEngine;

namespace CUAS.MMT
{

    //invoker class
    class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; } //singleton

        private Stack<ICommandU> buffer = new Stack<ICommandU>(); //command buffer

        private void Awake()
        {
            Instance = this;
        }

        //execute and add a command
        public void AddCommand(ICommandU command)
        {
            //Debug.Log("AddCommand");

            command.Execute();
            buffer.Push(command);

        }


        public void Undo()
        {

            if (buffer.Count == 0)
            {

                return;
            }

            var cmd = buffer.Pop();
            cmd.Undo();

        }

        public void Reset()
        {
            buffer.Clear();
        }

    }

}