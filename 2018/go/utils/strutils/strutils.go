package strutils

import (
	"strings"
)

func SplitLines(input string) (string_arr []string) {
	return strings.Split(input, "\n")
}

func SplitChars(input string) (char_arr []rune) {
	return []rune(input)
}
