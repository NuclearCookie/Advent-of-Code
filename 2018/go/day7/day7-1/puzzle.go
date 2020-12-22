package main

import (
	"fmt"
	"sort"
	"time"

	"github.com/nuclearcookie/aoc2018/day7/input"
	"github.com/nuclearcookie/aoc2018/utils/sliceutils"
)

type Instruction struct {
	Before []int
	After  []int
}

func (instruction *Instruction) AddBefore(beforeRune int) {
	instruction.Before = append(instruction.Before, beforeRune)
}

func (instruction *Instruction) AddAfter(afterRune int) {
	instruction.After = append(instruction.After, afterRune)
}

var instructions map[int]*Instruction
var processedInstructions map[int]bool

func main() {
	startTime := time.Now()
	instructions = make(map[int]*Instruction)
	processedInstructions = make(map[int]bool, len(instructions))
	data := input.GetSplit()
	parse(data)
	firstInstructions := findFirstInstructions()
	order := followInstructionsRecursive(firstInstructions, "")
	fmt.Printf("Instructions: %s\n", string(order))
	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
}

func parse(data []string) {
	var before string
	var current string
	for _, v := range data {
		fmt.Sscanf(v, "Step %s must be finished before step %s can begin.", &before, &current)
		currentRune := int(current[0])
		beforeRune := int(before[0])
		if _, exists := instructions[currentRune]; !exists {
			instructions[currentRune] = new(Instruction)
		}
		if _, exists := instructions[beforeRune]; !exists {
			instructions[beforeRune] = new(Instruction)
		}
		instructions[currentRune].AddBefore(beforeRune)
		instructions[beforeRune].AddAfter(currentRune)
	}
}

func findFirstInstructions() []int {
	var firstInstructions []int
	for k, v := range instructions {
		if v.Before == nil {
			firstInstructions = append(firstInstructions, k)
		}
	}
	return firstInstructions
}

func followInstructionsRecursive(potentiallyAvailableInstructions []int, order string) string {
	sort.Ints(potentiallyAvailableInstructions)
	var firstValidInstruction int
	var processedIndex int
	for i, v := range potentiallyAvailableInstructions {
		firstValidInstruction = v
		processedIndex = i
		if isValidInstruction(instructions[firstValidInstruction]) {
			processedInstructions[firstValidInstruction] = true
			order += string(v)
			break
		}
	}
	// remove processed index
	potentiallyAvailableInstructions = append(potentiallyAvailableInstructions[0:processedIndex], potentiallyAvailableInstructions[processedIndex+1:]...)
	for _, v := range instructions[firstValidInstruction].After {
		potentiallyAvailableInstructions = sliceutils.AppendIfMissing(potentiallyAvailableInstructions, v)

	}
	if len(potentiallyAvailableInstructions) == 0 {
		return order
	} else {
		return followInstructionsRecursive(potentiallyAvailableInstructions, order)
	}
}

func isValidInstruction(instruction *Instruction) bool {
	for _, v := range instruction.Before {
		if _, exists := processedInstructions[v]; !exists {
			return false
		}
	}

	return true
}
