package main

type eventSlice struct {
	sortedEvents []timeEvent
}

func (slice eventSlice) Less(i, j int) bool {
	return slice.sortedEvents[i].timestamp().Before(slice.sortedEvents[j].timestamp())
}

func (slice eventSlice) Len() int {
	return len(slice.sortedEvents)
}

func (slice eventSlice) Swap(i, j int) {
	temp := slice.sortedEvents[i]
	slice.sortedEvents[i] = slice.sortedEvents[j]
	slice.sortedEvents[j] = temp
}
