package main

import (
	"fmt"
	"time"
)

type changeGuard struct {
	time time.Time
	id   int
}

func (event changeGuard) timestamp() time.Time {
	return event.time
}

func (event changeGuard) String() string {
	return fmt.Sprintf("{time: %s, guard id: %d}\n", event.time.Format("Mon Jan _2 15:04"), event.id)
}
