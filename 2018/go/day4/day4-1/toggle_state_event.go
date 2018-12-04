package main

import (
	"fmt"
	"time"
)

type toggleState struct {
	time  time.Time
	awake bool
}

func (event toggleState) timestamp() time.Time {
	return event.time
}

func (event toggleState) String() string {
	return fmt.Sprintf("{time: %s, awake: %t}\n", event.time.Format("Mon Jan _2 15:04"), event.awake)
}
