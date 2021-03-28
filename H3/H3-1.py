import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import random


def create_first_population(n,cities,length):
  chroms=[]
  for i in range(n):
    chroms.append((random.sample(cities, length)))
  return chroms

#SOPIVUUSARVOT
def count_fitness_values(chroms,distances,length):
  #all_costiin kerätään kaikkien reittivaihtoehtojen km-määrät
  all_costs=[]
  for kromosomi in chroms:
    #käsittelyyn kromosomi kerrallaan:
    #total_cost:iin kerätään aina yhden kromosomin sopivuusarvo eli km-määrä:
    total_cost=0
    for i in range (length-1):
      from_city, to_city=kromosomi[i],kromosomi[i+1]
      total_cost=total_cost+distances[from_city][to_city]
    #lisätään vielä paluumatkan kustannus
    alku=kromosomi[0]
    total_cost=total_cost+distances[alku][to_city]
    all_costs.append(total_cost)
  dictionary={'Chromosomes':chroms,'Fitness':all_costs}
  #Rakennetaan kromosomeista/reiteistä ja niiden kustannuksista dataframe:
  city_fitness=pd.DataFrame(dictionary)
  return city_fitness

#VALINTA
def tournament(city_fitness,n):
  #Otetaan turnajaisiin 3 ehdokasta
  num_of_attendees=3
  parents=[]
  #Turnajaisia tehdään 20 kertaa (n=20 eli populaation koko)
  for i in range(n):
    attendees=city_fitness.sample(num_of_attendees)
    #laitetaan järjestykseen, ascending=nouseva järjestys, eli pienin arvo on ensimmäinen
    attendees.sort_values('Fitness',ascending=True,inplace=True)
    #Poimitaan pienimmän arvon omaava kromosomi talteen
    parents.append(attendees['Chromosomes'].iloc[0])
  return parents

#CROSSOVER/BREED
#Replacement on crossoverin "apuohjelma"
def replacement(x,y, length):
  for i in range(0,length):
    #jos y-kromosomissa on sellainen geeni jota ei löydy vielä x:stä, lisätään se x:ään
    if y[i] not in x:
      x.append(y[i])
  return x

def crossover(parents,n,length):
  new_offsprings=[]
  for j in range(0,n-1,2):
    mother=parents[j]
    father=parents[j+1]
    #Katsotaan tapahtuuko risteytys vai kloonaus
    if random.random()<p_c:
      #Risteytys, kohdasta ind eli indeksi, arvotaan satunnaisesti:
      ind=random.randint(1,length-2)
      #äiti-kromosomin alkuosa ja loppuosa isältä:
      mother_1=mother[0:ind]
      offspring_1= replacement(mother_1,father, length)
      #isä-kromosomin alkuosa ja loppuosa äidiltä:
      father_1=father[0:ind]
      offspring_2=replacement(father_1,mother, length)
      #Lisätään molemmat jälkeläiset uusien jälkeläisten listaan
      new_offsprings.append(offspring_1)
      new_offsprings.append(offspring_2)
    else:
      #Kloonaus:
      new_offsprings.append(mother)
      new_offsprings.append(father)
  return new_offsprings

def mutation(new_offsprings,length, p_m):
  mutated=[]
  for i in new_offsprings:
    c=random.random()
    # mutaation todennäköisyyttä imitoidaan arpomalla reaaliluku. Jos se on pienempi kuin mutaatio-tn,
    # silloin mutaatio tapahtuu. Muutoin ei.
    if p_m >=c:
      #kromosomissa tapahtuu mutaatio
      #arvotaan kaksi indeksiä, joiden geenit vaihtavat paikkoja
      new_genes=random.sample(range(0,length),2)
      first=new_genes[0]
      second=new_genes[1]
      #otetaan väliaikaisesti talteen 1. vaihdettava geeni
      temp=i[first]
      #Päivitetään 1. vaihdettavan geenin tilalle uusi
      i[first]=i[second]
      #ja tokan paikalle eka
      i[second]=temp
    mutated.append(i)
  return mutated

#Tämä aliohjelma/funktio sisältää toistettavat askeleet
def evolution (chroms, n, p_m, p_c,length):
  #1. Lasketaan kromosomeille sopivuusarvot
  city_fitness=count_fitness_values(chroms,distances,length)
  #2. valinta turnajaisilla:
  parents=tournament(city_fitness,n)
  #3. Risteytys:
  new_offsprings=crossover(parents,n,length)
  #4. Mutaatio
  new_population=mutation(new_offsprings,length, p_m)
  return new_population, city_fitness

kaupungit_nro = [0,1,2,3,4,5]
nimet = ['Ankkapurha', 'Jänishaikula', 'Kontu', 'Jeppelä', 'Peräkorpi', 'Koikkala']

distances = pd.read_csv('lentorahti.csv', delimiter=';', names = kaupungit_nro)


n = 100
p_c = 0.7
p_m = 0.001

l = 6

#Aloitetaan luomalla satunnainen alkupopulaatio
chroms=create_first_population(n,kaupungit_nro,l)
#print('Randomly selected chromosomes:', pd.DataFrame(chroms))

#Määrätään alussa kuinka monta kierrosta suoritetaan
#Eli kuinka monta sukupolvea luodaan
iterations=40
evolution_step=0
while evolution_step<=iterations:
  chroms,cost = evolution(chroms, n, p_m, p_c, l)
  evolution_step=evolution_step + 1
print('Tehtävä 1.')
print(pd.DataFrame(chroms))
print('Total cost of the route', cost['Fitness'].iloc[0],'km')


