using Godot;
using System.Collections.Generic;
using System.Linq;

namespace starcraftbuildtrainer.scripts
{
    public partial class MessageControl : Control
    {
        private readonly List<DynamicLabel> _messegeList = [];

        private float _messageHeight;
        Color _messageColor = new(1, 0, 0, 1);

        private const int MESSAGE_VERTICAL_RATIO = 5;
        private const float MESSAGE_FADE_RATE = 0.25f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _messageHeight = Size.Y / MESSAGE_VERTICAL_RATIO;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            foreach (var message in _messegeList)
            {
                //Fade Messages
                var color = message.Modulate;
                color.A -= (float)(MESSAGE_FADE_RATE * delta);

                //Remove Faded Messages
                if (color.A < 0)
                    RemoveMessage(message);
                else 
                    message.Modulate = color;
            }
        }

        public void DisplayMessage(string messageText)
        {
            //Make Message
            var newMessage = new DynamicLabel
            {
                Text = messageText,
                Position = new(0, Size.Y),
                Size = new(Size.X, _messageHeight),
                Modulate = _messageColor,
            };

            AddChild(newMessage);
            _messegeList.Add(newMessage);

            //Update Positions
            foreach (var message in _messegeList)
            {
                float yPosition = message.Position.Y - _messageHeight;
                message.Position = new(0, yPosition);
            }

            //Remove Out of Bounds
            DynamicLabel firstMessage = _messegeList.First();
            if (firstMessage.Position.Y < 0 )
            {
                RemoveMessage(firstMessage);
            }
        }

        private void RemoveMessage(DynamicLabel firstMessage)
        {
            RemoveChild(firstMessage);
            _messegeList.Remove(firstMessage);
        }
    }
}