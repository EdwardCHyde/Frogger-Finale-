using System.Collections.Generic;
using Unit04.Game.Casting;
using Unit04.Game.Services;
using System;
using Raylib_cs;



namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
        private KeyboardService keyboardService = null;
        private VideoService videoService = null;

        public int points = 0;
        public int yMinCar = 0;
        public int yMinLog = 0;
        public Casting.Color WHITE = new Casting.Color(225, 255, 225);
        
        

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this.keyboardService = keyboardService;
            this.videoService = videoService;
        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            videoService.OpenWindow();
            while (videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            Point velocity = keyboardService.GetDirection();
       

            List<Actor> log = cast.GetActors("log");
            List<Actor> artifacts = cast.GetActors("artifacts");



            int newdx = 3;
            int newdy = 0;
          
            
            Point newdirection = new Point(newdx, newdy);
            Point zeroDirection = new Point(newdy, newdy);

            
             
            robot.SetVelocity(velocity); 

            foreach (Actor actor in log)
            {

                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    robot.SetVelocity2(newdirection);

                }

        
            }

           
            //Car cars = (Car)cast.GetFirstActor("cars");
            List<Actor> car = cast.GetActors("cars");
            
           
            Car cars = new Car(new Point(5,0));
            cars.PrepareCars(newdirection);
            // Game over 
            foreach (Actor actor in artifacts)
            {
                Point robotPosition = robot.GetPosition();
                int x = robotPosition.GetX();
                int y = robotPosition.GetY();
                Rectangle RobotRecs = new Rectangle(x, y, 15, 15);

                Point actorPosition = actor.GetPosition();
                int x2 = actorPosition.GetX();
                int y2 = actorPosition.GetY();
                Rectangle actorRecs = new Rectangle(x2, y2, 30, 15);

                if (Raylib.CheckCollisionRecs(RobotRecs, actorRecs))
                {
               

                    
                    int MaxX = videoService.GetWidth();
                    int MaxY = videoService.GetHeight();
                    int X = MaxX / 2;
                    int Y = MaxY / 2;
                    Point position = new Point(X, Y);

                    Actor message = new Actor();
                    message.SetText("Game Over!");
                    message.SetPosition(position);
                    cast.AddActor("messages", message);
                    robot.SetText("X");

                    //set each car to white 
                    robot.SetColor(WHITE);
                    foreach (Actor actors in artifacts)
                    {
                        actors.SetColor(WHITE);
                    }

                    // set each log to white
                    foreach (Actor actors in log)
                    {
                        actors.SetColor(WHITE);
                    }


                }
            }



            
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            // Actor cars = cast.GetFirstActor("cars");
            List<Actor> log = cast.GetActors("log");
            List<Actor> car = cast.GetActors("artifacts");


            banner.SetText($"Score: {points}");
            int maxX = videoService.GetWidth();
            int maxY = videoService.GetHeight();
            robot.MoveNext(maxX, maxY);


            Random random = new Random();

            yMinLog = random.Next(0,39);

            if (yMinLog <= 20 && yMinLog >=8)
            {
                yMinCar = random.Next(0,yMinLog-7);
            }

            else if (yMinLog < 8)
            {
                yMinCar = random.Next(yMinLog+10,32);
            }

            else if (yMinLog <= 20 && yMinLog <=32)
            {
                yMinCar = random.Next(yMinLog-10,32);
            }

            else if (yMinLog > 31)
            {
                yMinCar = random.Next(0,yMinLog-10);
            }


            foreach (Actor actor in car)
            {
                actor.MoveNext(maxX, maxY);
                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    
                    Rock artifact = (Rock) actor;
                    int points = artifact.GetMessage();
                    this.points += points;
                    banner.SetText($"Score: {points}");


        


                }

                if (actor.GetPosition().GetY() == (maxY -10))
                {
                    Rock cars = (Rock) actor;
                    
                    
                    int x = cars.GetPosition().GetX();
                    int y = cars.GetPosition().GetY();
                    // Random random = new Random();
                    int ranX = random.Next(0, maxX) * 15;
                    
        

                    Point postition = new Point(ranX,y);
                    cars.SetPosition(postition);
                     
                }

                
            } 

            foreach(Actor cars in car)
            {
               
              
                

                int xVel = cars.GetVelocity().GetX()+10;
                int yVel = cars.GetVelocity().GetY();
                Point NewDirection = new Point(xVel, yVel);


         
               

            
                  
                if (robot.GetPosition().GetY() <10)
                {
                
                    cars.SetVelocity(NewDirection); 
                }
            }
           

            


            foreach (Actor actor in log)
            {
                actor.MoveNext(maxX, maxY);
              

                if (actor.GetPosition().GetY() == (maxY -10))
                {
                    Rock logs = (Rock) actor;
                    
                    
                    int x = logs.GetPosition().GetX();
                    int y = logs.GetPosition().GetY();
                    // Random random = new Random();
                    int ranX = random.Next(0, maxX) * 15;
                    
        

                    Point postition = new Point(ranX,y);
                    logs.SetPosition(postition);
                }

                
            } 



            //Set the the velocity to back to 0 once the frog leaves the log s
            int newdx = 5;
            int newdy = 0;
        
            Point newdirection = new Point(newdx, newdy);
            Point zeroDirection = new Point(newdy, newdy);
        

            foreach (Actor actor in log)
            {

            

                if (robot.GetPosition()!= actor.GetPosition())
                {
                    robot.SetVelocity2(zeroDirection); 
                }
            }

            yMinLog = random.Next(0,39);

            if (yMinLog <= 20 && yMinLog >=8)
            {
                yMinCar = random.Next(0,yMinLog-7);
            }

            else if (yMinLog < 8)
            {
                yMinCar = random.Next(yMinLog+10,32);
            }

            else if (yMinLog <= 20 && yMinLog <=32)
            {
                yMinCar = random.Next(yMinLog-10,32);
            }

            else if (yMinLog > 31)
            {
                yMinCar = random.Next(0,yMinLog-10);
            }

            if (robot.GetPosition().GetY() <10)
            {

                foreach (Actor cars in car)
                {
                    int x = random.Next(0,60)*15;
                    int y = random.Next(yMinCar,yMinCar+5)*15;
                    Point position = new Point(x,y);
                    cars.SetPosition(position);

                    int xVel = cars.GetVelocity().GetX();
                    int yVel = cars.GetVelocity().GetY();

                    Point newCarVel = new Point (xVel, yVel);
                    cars.SetVelocity(newCarVel);

                }

                 foreach (Actor logs in log)
                {
                    int x = random.Next(0,60)*15;
                    int y = random.Next(yMinLog,yMinLog+5)*15;
                    Point position = new Point(x,y);
                    logs.SetPosition(position);

                    int xVel = logs.GetVelocity().GetX();
                    int yVel = logs.GetVelocity().GetY();

                    Point newLogVel = new Point (xVel, yVel);
                    logs.SetVelocity(newLogVel);

                }
               
               




            }


      




        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            videoService.ClearBuffer();
            videoService.DrawActors(actors);
            videoService.FlushBuffer();

        }

    }
}



