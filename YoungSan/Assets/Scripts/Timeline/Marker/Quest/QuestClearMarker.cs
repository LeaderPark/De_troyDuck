using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("Quest/QuestClearMarker")]
public class QuestClearMarker : Marker, INotification
{
    public PropertyName id { get { return new PropertyName(); } }

    public Quest quest;
    
}

