import md5
input = "bgvyzdsv"
iterator = 0
while True:
    m = md5.new()
    m.update(input)
    m.update( str(iterator) )
    result = m.hexdigest()
    if result.startswith( '000000' ):
        print( result )
        print( iterator )
        break
    iterator += 1
