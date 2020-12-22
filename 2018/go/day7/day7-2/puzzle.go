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
var totalSeconds = -1
var totalWorkers = 5

func main() {
	startTime := time.Now()
	instructions = make(map[int]*Instruction)
	processedInstructions = make(map[int]bool, len(instructions))
	workers = make([]Worker, totalWorkers)
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
	var readyWorkers []*Worker
	// :NOTE: workers need to start working at the same tim. Not possible with current design.
	// Needs outer function to update the workers, then check to give work to all available workers.
	for readyWorkers == nil {
		updateWorkers()
		readyWorkers = getAvailableWorkers()
		for _, v := range readyWorkers {
			if v.timeRemaining == 0 && v.workingOnInstruction != 0 {
				order += string(v.workingOnInstruction)
				for _, v := range instructions[v.workingOnInstruction].After {
					potentiallyAvailableInstructions = sliceutils.Ints.AppendIfMissing(potentiallyAvailableInstructions, v)
				}
				if len(potentiallyAvailableInstructions) == 0 {
					return order
				}
			}
		}
	}
	sort.Ints(potentiallyAvailableInstructions)
	var firstValidInstruction int
	var processedIndex int
	var indicesToRemove []int
	for i, v := range potentiallyAvailableInstructions {
		firstValidInstruction = v
		processedIndex = i
		if len(readyWorkers) > 0 {
			if isValidInstruction(firstValidInstruction) {
				processedInstructions[firstValidInstruction] = true
				work(readyWorkers[0], firstValidInstruction)
				readyWorkers = readyWorkers[1:]
				indicesToRemove = append(indicesToRemove, processedIndex)
			}
		}
	}

	sort.Ints(indicesToRemove)
	for i := len(indicesToRemove) - 1; i >= 0; i-- {
		// for _, v := range indicesToRemove {
		potentiallyAvailableInstructions = sliceutils.Ints.Remove(potentiallyAvailableInstructions, indicesToRemove[i])
	}

	return followInstructionsRecursive(potentiallyAvailableInstructions, order)
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
		for _, worker := range workers {
			if worker.workingOnInstruction == v && worker.timeRemaining > 0 {
				return false
			}
		}
	}

	return true
}

func updateWorkers() {
	for i := range workers {
		workers[i].timeRemaining--
	}
	totalSeconds++
}
func getAvailableWorkers() []*Worker {
	var result []*Worker
	for i, v := range workers {
		if v.timeRemaining <= 0 {
			result = append(result, &workers[i])
		}
	}
	return result
}

func work(worker *Worker, instruction int) {
	worker.workingOnInstruction = instruction
	worker.timeRemaining = 60 + instruction - 'A' + 1
}
