// Fill out your copyright notice in the Description page of Project Settings.


#include "HealthOrb.h"
#include "HealthComponent.h"
#include "Components/SphereComponent.h"

// Sets default values
AHealthOrb::AHealthOrb()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	CollisionSphere = CreateDefaultSubobject<USphereComponent>(TEXT("CollisionBox"));
	RootComponent = CollisionSphere;

	CollisionSphere->SetGenerateOverlapEvents(true);
	CollisionSphere->OnComponentBeginOverlap.AddDynamic(this, &AHealthOrb::OnOverlapBegin);

}

// Called when the game starts or when spawned
void AHealthOrb::BeginPlay()
{
	Super::BeginPlay();
	
}

void AHealthOrb::OnOverlapBegin(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	UHealthComponent* Health = OtherActor->FindComponentByClass<UHealthComponent>();
	if (Health) {
		Health -> AddHealth(AddedHealth);
		Destroy();
	}
	

}

// Called every frame
void AHealthOrb::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

