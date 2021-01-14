using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction   // направление движения
	{
        Up,		// вверх
        Down,	// вниз
        Left,   // влево
		Right   // вправо
	}

    public class Snake : Drawable           // класс Snake определяет сущность змейки, реализует интерфес Drawable, чтобы можно было её отрисовывать
	{
        public event EventHandler EatFruit = (object sender, EventArgs e) => { };   // событие, когда змея съедает фрукт

		public Texture Texture { get => tile.Texture; set => tile.Texture = value; }    // текстура змеи
		public bool IsCollide { get; private set; }                                     // статус змеи, было ли столкновение с самим собой

		private List<Vector2i> segments;		// список позиций каждого сегмента змеи
		private Sprite tile;                    // спрайт змеи
		private Direction dir;                  // текущее направление движения

		public Snake() : this(new Vector2i(0,0))
        {

        }
        public Snake(Vector2i startPosition)    // в качестве параметра принимает сартовую позицию
		{
            Restart(startPosition);             // производим сброс всех параметров

			tile = new Sprite();
        }

        public void Update(Vector2i size, Fruit fruit)  // обновление поведения змеи
		{
            Control();                          // управление змеёй

			// сдвиг змеи вслед за головой
			for (int i = segments.Count - 1; i > 0; i--)    // перебираем сегменты от конца до головы не включая
			{
                segments[i] = segments[i - 1];              // позиции текущего сегмента присваивается позиция следующего сегмента
			}

			// движение змеи
			switch (dir)
            {
                case Direction.Up:      segments[0] -= new Vector2i(0, 1); break;	// смещаем позицию головы по координате Y вниз
                case Direction.Down:    segments[0] += new Vector2i(0, 1); break;	// смещаем позицию головы по координате Y вверх
                case Direction.Left:    segments[0] -= new Vector2i(1, 0); break;	// смещаем позицию головы по координате X влево
                case Direction.Right:   segments[0] += new Vector2i(1, 0); break;   // смещаем позицию головы по координате X вправо
			}

            Teleportation(size);	// телепортация змеи при достижении краёв карты

            if (segments[0] == fruit.Position)      // если координаты головы змеи совпадает с позицией фрукта
			{
				AddSegment();						// добавляем новый сегмен
				fruit.Restart(size);                // сбрасываем фрукт
				EatFruit(this, EventArgs.Empty);    // возбуждаем событие того, что змея съела фрукт

			}

            CheckCollision();                       // проверка на столкновение с самим собой
		}

        public void Restart(Vector2i startPosition)     // сброс змеи
		{
            dir = Direction.Right;                      // устанавливаем напрвавление
			IsCollide = false;                          // сбрасываем значение

			segments = new List<Vector2i>();            // создаём новый список сегментов
			for (int i = 0; i < 3; i++)                 // создаём 3 новых сегмента
			{
                segments.Add(new Vector2i(startPosition.X - i, startPosition.Y));   // добавляем сегмент со смещением относительно стартовой позиции по координате X
			}
        }

        private void Control()      // управление змеёй
		{
			// если нажата клавиша и змея не движется в противоположную выбранному направлению	устанавливаем выбранное направление
			if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && dir != Direction.Right)				dir = Direction.Left;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && dir != Direction.Left)		dir = Direction.Right;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && dir != Direction.Down)			dir = Direction.Up;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && dir != Direction.Up)			dir = Direction.Down;
        }

        private void AddSegment()   // добавление нового сегмента
		{
            int last = segments.Count - 1;									// получаем индекс последнего сегмента
			segments.Add(new Vector2i(segments[last].X, segments[last].Y)); // добавляем новый сегмент с позицией равной последнему сегменту
		}

        private void Teleportation(Vector2i size)   // телепортация змеи
		{
			// если голова змеи достигает края поля		то переносим её в противоположный конец
			if (segments[0].X >= size.X)				segments[0] = new Vector2i(0, segments[0].Y);
            if (segments[0].X < 0)         				segments[0] = new Vector2i(size.X, segments[0].Y);
            if (segments[0].Y >= size.Y)   				segments[0] = new Vector2i(segments[0].X, 0);
            if (segments[0].Y < 0)         				segments[0] = new Vector2i(segments[0].X, size.Y);
        }

        private void CheckCollision()           // проверка на столкновение с самим собой
		{
            for (int i = 1; i < segments.Count; i++)    // перебираем все сегменты кроме первого(головы)
			{
                if (segments[0] == segments[i])     // если позиция головы совпадает с позицией одного из сегментов
					IsCollide = true;               // то было столкновение
			}
        }

        public void Draw(RenderTarget target, RenderStates states)      // отрисовка змеи
		{
            foreach (var node in segments)      // перебираем все сегменты
			{
                tile.Position = new Vector2f(					// устанавливаем позицию каждого сегмента змеи
					node.X * tile.GetGlobalBounds().Width,      // по координате X
					node.Y * tile.GetGlobalBounds().Height);    // по координате Y
																// домножая номер клетки матрицы на ширину и высоту одной плитки соответственно
				target.Draw(tile, states);                      // отрисовываем плитку
																// и так для каждой
			}
		}
    }
}
