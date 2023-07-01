//using Elevator.CommandResult;
//using Elevator.Commands;
//using Elevator.Interfaces;
//using Elevator.Manager;

//namespace Elevator.CommandHandler
//{
//    public class LiftRequestResponseHandler : ICommandHandler<LiftRequestCommand, LiftRequestResult>
//    {

//        public LiftRequestResponseHandler()
//        {

//        }
//        public LiftRequestResult Handle(LiftRequestResult command)
//        {
//			var liftRequestcommand = new LiftRequestResult(fromFloor, toFloor);

//			eventHandler.PublishMessage(liftRequestcommand);
//		}

//    }
//}
