package main

import (
	"fmt"
	"sort"
	"time"

	"github.com/nuclearcookie/aoc2018/day7/input"
	"github.com/nuclearcookie/aoc2018/utils/sliceutils"
)

type Instruction struct {
	Before, After []int
}

type Worker struct {
	workingOnInstruction, timeRemaining int
}

func (instruction *Instruction) AddBefore(beforeRune int) {
	instruction.Before = append(instruction.Before, beforeRune)
}

func (instruction *Instruction) AddAfter(afterRune int) {
	instruction.After = append(instruction.After, afterRune)
}

var instructions map[int]*Instruction
var processedInstructions map[int]bool
var workers []Worker
var totalSeconds int

func main() {
	startTime := time.Now()
	instructions = make(map[int]*Instruction)
	processedInstructions = make(map[int]bool, len(instructions))
	workers = make([]Worker, 5)
	data := input.GetSplit()
	parse(data)
	firstInstructions := findFirstInstructions()
	order := followInstructionsRecursive(firstInstructions, "")
	fmt.Printf("Instructions: %s (time: %d)\n", string(order), totalSeconds)
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
	worker := -1
	// :NOTE: workers need to start working at the same tim. Not possible with current design.
	// Needs outer function to update the workers, then check to give work to all available workers.
	for worker == -1 {
		updateWorkers()
		worker = getAvailableWorker()
		availableWorker := workers[worker]
		if availableWorker.timeRemaining == 0 && availableWorker.workingOnInstruction != 0 {
			order += string(availableWorker.workingOnInstruction)
		}
	}
	sort.Ints(potentiallyAvailableInstructions)
	var firstValidInstruction int
	var processedIndex int
	for i, v := range potentiallyAvailableInstructions {
		firstValidInstruction = v
		processedIndex = i
		if isValidInstruction(firstValidInstruction) {
			processedInstructions[firstValidInstruction] = true
			work(worker, firstValidInstruction)
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

func isValidInstruction(instruction int) bool {
	for _, v := range workers {
		if v.workingOnInstruction == instruction && v.timeRemaining > 0 {
			return false
		}
	}
	currentInstruction := instructions[instruction]
	for _, v := range currentInstruction.Before {
		if _, exists := processedInstructions[v]; !exists {
			return false
		}
	}

	return true
}

func updateWorkers() {
	for _, v := range workers {
		v.timeRemaining--
	}
	totalSeconds++
}
func getAvailableWorker() int {
	for i, v := range workers {
		if v.timeRemaining <= 0 {
			return i
		}
	}
	return -1
}

func work(workerID, instruction int) {
	workers[workerID].workingOnInstruction = instruction
	workers[workerID].timeRemaining = instruction - 'A' + 1
}
