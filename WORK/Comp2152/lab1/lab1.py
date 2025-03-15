# Sample Coding Questions 01 Week 01.
# Matthew Zygiel

array = [1, 4, 7, 9]

a = 1
b = 2
c = 3
d = 4

e = ((a - ((b ** c) // d)) + (a % c))

print("The value of e is ", e)

temperature = 32.6
print("The temperature today is: {:.3f} degrees celsius". format(temperature))
# "{:.3f}" makes sure that there are 3 decimal places

userAge = int(input("Enter your age."))

userAge += 22

print("Now showing the shop items filtered by age:", userAge)
