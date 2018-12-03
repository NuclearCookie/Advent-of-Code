package sliceutils

func FilterString(a []string, f func(int, string) bool) []string {
	vsf := make([]string, 0)
	for i, v := range a {
		if f(i, v) {
			vsf = append(vsf, v)
		}
	}
	return vsf
}

func FilterRune(a []rune, f func(int, rune) bool) []rune {
	vsf := make([]rune, 0)
	for i, v := range a {
		if f(i, v) {
			vsf = append(vsf, v)
		}
	}
	return vsf
}
