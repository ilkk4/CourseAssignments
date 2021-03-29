import random, math, time

# evaluate gene distance from given value 78
def fitness(gene):
    v = calculate(gene)
    best = 78

    if v > best:
        return v - best
    elif best > v:
        return best - v
    else:
        return 0

# evaluate the given expression:
# 5A+2B-7C+4D
def calculate(gene):
    return 5*gene[0]+2*gene[1]-7*gene[2]+4*gene[3]

# gene: [A,B,C,D]
def createGene(k=4):
    numbers = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20]
    return random.choices(numbers,k=k)

#  population: n x [A,B,C,D]
def createPopulation(n):
    population = list()
    for i in range(n):
        population.append(createGene())
    return population

# population-wide fitness (avg)
def populationFitness(genes):
    n = len(genes)
    pop_fitness = 0
    for g in genes:
        pop_fitness += fitness(g)
    return pop_fitness/n


def ranked(gene):
    return (gene, fitness(gene))

def rankedList(genes):
    l = list()
    for g in genes:
        l.append(ranked(g))
    return l


# "best" gene, aka lowest fitness value
def minGene(genes):
    min = fitness(genes[0])
    minGene = genes[0]

    for g in genes[1:]:
        f = fitness(g)
        if f < min:
            min = f
            minGene = g
    return minGene

# select k random genes, return best of selected
def tourney_selection(population, k=3):
    attendees = random.sample(population, k=k)
    return minGene(attendees)

# crossOver:
# tourney-select 2 parents
# crossBreed / pass genes forward based on
# pC = "probability of crossOver"
def crossOver(population, pC):
    parent_1 = tourney_selection(population)
    parent_2 = tourney_selection(population)

    safety = 0
    while parent_2 == parent_1 and safety < 10:
        parent_2 = tourney_selection(population)
        safety += 1

    child_1 = parent_1
    child_2 = parent_2

    if random.random() < pC:
        child_1 = crossBreed(parent_1,parent_2)
        child_2 = crossBreed(parent_2, parent_1)
    return child_1, child_2

# splice together a&b at random index
def crossBreed(a,b):
    i = random.randint(1, 2)
    child = a[:i]+b[i:]
    return child

# apply mutations to population based on
# pM = "probability of mutation"
# mutation : randomize randomly selected chromosome (A-D)
def mutate(population, pM):
    for p in population:
        if random.random() < pM:
            i_1 = random.randint(0,3)
            p[i_1] = random.randint(1,19)

    return population



n = 16
p_Crossover = 0.7
p_Mutation = 0.001

iter = 10000

population = createPopulation(n)
print(rankedList(population))
print('Initial population-wide fitness:', populationFitness(population))
print('Best gene:', ranked(minGene(population)), 'solved:', calculate((minGene((population)))))

for i in range(iter):
    new_population = list()
    while(len(new_population) < n):
        child_1, child_2 = crossOver(population, p_Crossover)
        new_population.append(child_1)
        new_population.append(child_2)

    new_population = mutate(new_population, p_Mutation)

    pf = populationFitness((new_population))
    if pf == 0:
        print('Solved! Exiting at iteration', i)
        break

    population = new_population

print('N:', len(new_population), ':', rankedList(new_population))
print('Final population-wide fitness:', populationFitness(new_population))
print('Best gene:', ranked(minGene(new_population)), 'solved:', calculate((minGene((new_population)))))
