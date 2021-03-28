import random, math, time

def fitness(gene):
    A = gene[0]
    B = gene[1]
    C = gene[2]
    D = gene[3]
    return math.fabs((5*A+2*B-7*+4*D)-78)

def createGene():
    numbers = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20]
    return random.choices(numbers,k=4)

def createPopulation(n):
    population = list()
    for i in range(n):
        population.append(createGene())
    return population

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



def minGene(genes):
    min = fitness(genes[0])
    minGene = genes[0]

    for g in genes[1:]:
        f = fitness(g)
        if f < min:
            min = f
            minGene = g
    return minGene

def tourney_selection(population, k=5):
    attendees = random.sample(population, k=k)
    return minGene(attendees)


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

def crossBreed(a,b):
    i = random.randint(1, 2)
    child = a[:i]+b[i:]
    return child

def mutate(population, pM):
    for p in population:
        if random.random() < pM:
            i_1 = random.randint(0,3)
            p[i_1] = random.randint(1,19)

    return population





n = 16
p_Crossover = 0.8
p_Mutation = 0.01

iter = 50000

population = createPopulation(n)
print(rankedList(population))
print('Initial population-wide fitness:', populationFitness(population))
print('Best gene:', minGene(population), fitness(minGene(population)))

for i in range(iter):
    new_population = list()
    while(len(new_population) < n):
        child_1, child_2 = crossOver(population, p_Crossover)
        new_population.append(child_1)
        new_population.append(child_2)

    new_population = mutate(new_population, p_Mutation)
    population = new_population

print(rankedList(new_population))
print('Final population-wide fitness:', populationFitness(new_population))
print('Best gene:', ranked(minGene(new_population)))