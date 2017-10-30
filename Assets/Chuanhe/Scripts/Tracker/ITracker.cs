using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITracker {

	Dictionary<string, string> eventBlacklist { get; set; }
	void Initialize();
	void TrackEvent (string name, Dictionary<string, object> data);
}
