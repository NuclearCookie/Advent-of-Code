package main

import (
	"fmt"
	"math"
	"strings"
	"time"
	"unicode"

	"github.com/nuclearcookie/aoc2018/utils/strutils"

	"github.com/nuclearcookie/aoc2018/day5/input"
)

type stripResult struct {
	strippedPolymer []rune
	strippedUnit    rune
}

func main() {
	startTime := time.Now()
	runes := strutils.SplitChars(input.Get())
	c := make(chan stripResult, 'z'-'a')
	for char := 'a'; char < 'z'; char++ {
		go stripUnit(runes, char, c)
	}
	lowest := math.MaxInt32
	for index := 'a'; index < 'z'; index++ {
		result := <-c
		length := len(result.strippedPolymer)
		if length < lowest {
			lowest = length
		}
	}
	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Shortest polymer length: %d", lowest)
}

func stripUnit(runes []rune, char rune, c chan stripResult) {
	str := string(runes)
	str = strings.Replace(str, string(char), "", -1)
	str = strings.Replace(str, string(unicode.ToUpper(char)), "", -1)
	c <- stripResult{processPolymer([]rune(str)), char}
}

func processPolymer(runes []rune) []rune {
	previousRune := runes[0]
	index := 1
	for index < len(runes)-1 {
		currentRune := runes[index]
		if previousRune-32 == currentRune || previousRune+32 == currentRune {
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
	return runes
}
