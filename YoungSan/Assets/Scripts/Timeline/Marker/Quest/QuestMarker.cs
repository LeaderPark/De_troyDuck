using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.ComponentModel;

[DisplayName("Quest/QuestMarker")]
public class QuestMarker : Marker, INotification
{
    public PropertyName id { get { return new PropertyName(); } }

    public Quest quest;
    
}
