using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Fruit : Drawable       // класс Fruit определяет сущность фрукта, реализует интерфес Drawable, чтобы можно было её отрисовывать
	{
        public Texture Texture { get => tile.Texture; set => tile.Texture = value; }    // текстура фрукта
		public Vector2i Position { get; private set; }                                  // текущая позиция

		private Sprite tile;                                                            // спрайт фрукта

		public Fruit() : this(new Vector2i(0,0))
        {
            
        }
        public Fruit(Vector2i sizeField)
        {
            tile = new Sprite();
            Restart(sizeField);     // рестартим
		}

        public void Restart(Vector2i sizeField) // рестарт фрукта
		{
            Position = new Vector2i(Program.random.Next(0, sizeField.X), Program.random.Next(0, sizeField.Y));  // генерируем произвольную позицию относительно рамера игрового поля
		}

        public void Draw(RenderTarget target, RenderStates states)  // отрисовка фрукта
		{
            tile.Position = new Vector2f(						// устанавливаем позицию фрукта
				Position.X * tile.GetGlobalBounds().Width,      // по координате X
				Position.Y * tile.GetGlobalBounds().Height);	// по координате Y
																// домножая номер клетки матрицы на ширину и высоту одной плитки соответственно
            target.Draw(tile, states);                          // отрисовка фрукта
		}
    }
}
