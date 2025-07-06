// Fill out your copyright notice in the Description page of Project Settings.


#include "Lava.h"
#include "Components/BoxComponent.h"
#include "HealthComponent.h"
#include "TimerManager.h"

// Sets default values
ALava::ALava()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

    CollisionBox = CreateDefaultSubobject<UBoxComponent>(TEXT("CollisionBox"));
    RootComponent = CollisionBox;

    CollisionBox->SetGenerateOverlapEvents(true);
    CollisionBox->OnComponentBeginOverlap.AddDynamic(this, &ALava::OnOverlapBegin);
    CollisionBox->OnComponentEndOverlap.AddDynamic(this, &ALava::OnOverlapEnd);

}

// Called when the game starts or when spawned
void ALava::BeginPlay()
{
	Super::BeginPlay();
	
}

void ALava::OnOverlapBegin(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
    if (!OtherActor) return;

    // Start timer to apply damage repeatedly
    FTimerHandle& Timer = DamageTimers.FindOrAdd(OtherActor);
    GetWorldTimerManager().SetTimer(
        Timer,
        FTimerDelegate::CreateUObject(this, &ALava::ApplyDamage, OtherActor),
        DamageInterval,
        true
    );
    
}

void ALava::OnOverlapEnd(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex)
{
    if (!OtherActor) return;

    // Stop the timer
    if (DamageTimers.Contains(OtherActor))
    {
        GetWorldTimerManager().ClearTimer(DamageTimers[OtherActor]);
        DamageTimers.Remove(OtherActor);
    }
}

void ALava::ApplyDamage(AActor* Actor)
{
    if (!Actor) return;

    UHealthComponent* Health = Actor->FindComponentByClass<UHealthComponent>();
    if (Health && !Health->IsDead())
    {
        Health->TakeDamage(Damage);
    }
}

// Called every frame
void ALava::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

