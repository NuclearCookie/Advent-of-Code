package main

import (
	"fmt"
	"time"

	"github.com/nuclearcookie/aoc2018/utils/strutils"

	"github.com/nuclearcookie/aoc2018/day5/input"
)

func main() {
	startTime := time.Now()
	runes := strutils.SplitChars(input.Get())
	previousRune := runes[0]
	index := 1
	for index < len(runes)-1 {
		currentRune := runes[index]
		if previousRune-32 == currentRune || previousRune+32 == currentRune {
			// fmt.Printf("REMOVING %s, %s\n", string(previousRune), string(currentRune))
			runes = append(runes[:index-1], runes[index+1:]...)
			index -= 2
			if index < 1 {
				index = 1
			}
			previousRune = runes[index-1]
		} else {
			index++
			previousRune = currentRune
		}
	}

	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Polymer length: %d", len(runes))
}
