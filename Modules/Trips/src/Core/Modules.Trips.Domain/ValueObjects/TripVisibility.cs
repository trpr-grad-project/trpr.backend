using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Trips.Domain.ValueObjects
{
    public enum TripPublishMode
    {
        DirectPublish,
        Bidding
    }
    // when creating a trip add the auto approve joiners option 
    // allow the trip owner to review the people joining and approve or disaprove them 
    // add a flag to the tripparticipant to know if he is approved or not 
    // after approving someone if the number of the approved equals the max participants 
    // remove the pending users 
    // check if the user balance allows him to join a trip and make a payment module
    // make the transaction pending untill the user is approved or gets rejected 
    // move the trip to published after a bidding is approved 
    // move to completed automatically if the the number of participants equals the max participant
    // when the trip is completed move the charge from pending to the guide pending balance 
    // if the trip is canceled apply a cancelation fee after that time
    // create invitation links
    public enum TripStatus
    {
        UnderReview = 1,
        Published = 2,
        Bidding = 3,
        Completed = 4,
        Started = 5,
        Finished = 6,
        Rejected = 7,
        Canceled = 8,
    }
    public enum TripVisibility
    {
        Private = 0,
        Public = 1,
    }
}
